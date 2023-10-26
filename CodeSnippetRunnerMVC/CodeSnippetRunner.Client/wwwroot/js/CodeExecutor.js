class CodeExecutor {
    constructor(editor) {
        this.editor = editor;
        this.executeBlobFile = this.executeBlobFile.bind(this);
        this.deleteBlobFile = this.deleteBlobFile.bind(this);
        this.currentEditingBlobName = null;
        document.addEventListener("DOMContentLoaded", this.refreshBlobFileList.bind(this));
    }

    executeCode() {
        this.clearOutputs();

        const code = this.editor.getValue();

        fetch('/api/executor/snippet', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({inputCode: code})
        })
            .then(response => {
                if (response.headers.get("content-type")?.includes("application/json")) {
                    return response.json();
                }
                return response.text().then(text => {
                    throw new Error(text);
                });
            })
            .then(data => {
                document.getElementById('result').textContent = JSON.stringify(data.result, null, 2);
                document.getElementById('consoleOutput').textContent = data.consoleOutput;
                if (data.errorOutput) {
                    throw new Error(data.errorOutput);
                }
            })
            .catch(error => {
                console.error("Error executing code:", error);
                document.getElementById('errorOutput').textContent = error.message;
            })
            .finally(() => {
                this.refreshBlobFileList();
            });
    }

    uploadAndExecuteCodeFromFile() {
        this.clearOutputs();

        const fileInput = document.getElementById('codeFileInput');
        const file = fileInput.files[0];

        if (!file || (file.type !== "text/plain" && file.name.slice(-3) !== ".cs")) {
            alert("Please select a valid .txt or .cs file.");
            return;
        }

        if (file.size > 10_240) { // 10 KB limit
            alert("File size should not exceed 10 KB.");
            return;
        }

        const formData = new FormData();
        formData.append("CodeFile", file);

        fetch('/api/executor/file', {
            method: 'POST',
            body: formData
        })
            .then(response => {
                if (response.headers.get("content-type")?.includes("application/json")) {
                    return response.json();
                }
                return response.text().then(text => {
                    throw new Error(text);
                });
            })
            .then(data => {
                document.getElementById('result').textContent = JSON.stringify(data.result, null, 2);
                document.getElementById('consoleOutput').textContent = data.consoleOutput;
                this.editor.setValue(data.content);
                this.editor.clearSelection();
                if (data.errorOutput) {
                    throw new Error(data.errorOutput);
                }
            })
            .catch(error => {
                console.error("Error executing code:", error);
                document.getElementById('errorOutput').textContent = error.message;
            })
            .finally(() => {
                this.refreshBlobFileList();
            });
    }

    updateFileName() {
        const fileInput = document.getElementById('codeFileInput');
        const selectedFileName = document.getElementById('selectedFileName');
        const fileNameSection = document.querySelector('.file-name-section');
        document.getElementById("fileNameSection").hidden = false;

        if (fileInput.files.length > 0) {
            const fileName = fileInput.files[0].name;
            selectedFileName.textContent = fileName;
            fileNameSection.style.display = 'block'; 
        } else {
            selectedFileName.textContent = '';
            fileNameSection.style.display = 'none'; 
        }
    }

    clearOutputs() {
        document.getElementById('result').textContent = '';
        document.getElementById('consoleOutput').textContent = '';
        document.getElementById('errorOutput').textContent = '';
    }

    refreshBlobFileList() {
        fetch('/api/snippets')
            .then(response => response.json())
            .then(data => {
                const blobFilesList = document.getElementById('blobFilesList');
                blobFilesList.innerHTML = '';

                data.forEach(file => {
                    const fileDiv = document.createElement('div');

                    const editButton = document.createElement('button');
                    if (this.currentEditingBlobName === file) {
                        editButton.textContent = 'Opened';
                        editButton.className = 'opened-blob-button'; 
                    } else {
                        editButton.textContent = 'Edit';
                        editButton.className = 'edit-blob-button';
                    }
                    editButton.style.marginRight = '5px';
                    editButton.addEventListener('click', () => this.loadFileForEditing(file));
                    fileDiv.appendChild(editButton);
                    
                    const deleteButton = document.createElement('button');
                    deleteButton.textContent = 'Delete';
                    deleteButton.className = 'delete-blob-button';
                    deleteButton.addEventListener('click', () => this.deleteBlobFile(file));
                    fileDiv.appendChild(deleteButton);
                    
                    const fileNameText = document.createTextNode(` ${file} `);
                    fileDiv.insertBefore(fileNameText, editButton);

                    blobFilesList.appendChild(fileDiv);
                });
            })
            .catch(error => {
                console.error("Error fetching blob file list:", error);
            });
    }

    executeBlobFile(blobFileName) {
        this.clearOutputs();

        fetch('/api/executor/storage-file', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({blobFileName: blobFileName})
        })
            .then(response => {
                if (response.headers.get("content-type")?.includes("application/json")) {
                    return response.json();
                }
                return response.text().then(text => {
                    throw new Error(text);
                });
            })
            .then(data => {
                document.getElementById('result').textContent = JSON.stringify(data.result, null, 2);
                document.getElementById('consoleOutput').textContent = data.consoleOutput;
                this.editor.setValue(data.content);
                this.editor.clearSelection();
                if (data.errorOutput) {
                    throw new Error(data.errorOutput);
                }
            })
            .catch(error => {
                console.error("Error executing code:", error);
                document.getElementById('errorOutput').textContent = error.message;
            });
    }

    loadFileForEditing(blobFileName) {
        this.clearOutputs();
        
        fetch(`/api/executor/storage-file`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ blobFileName: blobFileName })
        })
            .then(response => response.json())
            .then(data => {
                this.editor.setValue(data.content);
                this.editor.clearSelection();
                this.refreshBlobFileList();
                this.currentEditingBlobName = blobFileName;
            })
            .catch(error => console.error("Error loading blob for editing:", error));
        
        document.getElementById("saveToStorageButton").hidden = true;
        document.getElementById("infoTextBlock").hidden = true;
        document.getElementById("chooseFileButton").hidden = true;
        document.getElementById("executeFileButton").hidden = true;

        document.getElementById("deleteBlobButton").hidden = false;
        document.getElementById("saveBlobButton").hidden = false;
        document.getElementById("resetIcon").hidden = false;

        document.getElementById("selectedFileName").textContent = blobFileName;
        document.getElementById("fileNameSection").style.display = "block";
    }

    saveEdits() {
        const updatedContent = this.editor.getValue();

        if (!this.currentEditingBlobName) {
            alert("No blob is currently being edited.");
            return;
        }

        fetch(`/api/snippets/${encodeURIComponent(this.currentEditingBlobName)}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ content: updatedContent })
        })
            .then(response => {
                if (response.ok) {
                    this.refreshBlobFileList();
                    alert("Changes saved successfully.");
                } else {
                    return response.text().then(text => { throw new Error(text); });
                }
            })
            .catch(error => console.error("Error saving changes:", error));
    }

    saveToBlob() {
        const codeContent = this.editor.getValue();
        const defaultFileName = "mySnippet";
        const userFileName = prompt("Please enter a name for the snippet:", defaultFileName);

        if (!userFileName || userFileName.trim() === "") {
            alert("File name is required");
            return;
        }

        fetch('/api/executor/save-snippet', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ inputCode: codeContent, fileName: userFileName + ".txt"})
        })
            .then(response => {
                if (response.ok) {
                    alert("Saved successfully to blob storage.");
                    this.refreshBlobFileList();
                } else {
                    return response.text().then(text => { throw new Error(text); });
                }
            })
            .catch(error => console.error("Error saving to blob:", error));
    }

    deleteBlobFile(blobFileName = this.currentEditingBlobName) {
        if (!blobFileName) {
            console.error("No file name provided to deleteBlobFile");
            return;
        }

        if (!confirm('Are you sure you want to delete this file?')) {
            return;
        }

        fetch(`/api/snippets/${encodeURIComponent(blobFileName)}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error(`Server error: ${response.statusText}`);
                }
                this.refreshBlobFileList();
            })
            .catch(error => {
                console.error("Error deleting blob file:", error);
            });
    }

    downloadEditorContent() {
        const codeContent = this.editor.getValue();

        fetch('/api/files/download-code', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ content: codeContent })
        })
            .then(response => response.blob())
            .then(blob => {
                const url = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.style.display = 'none';
                a.href = url;
                a.download = 'code.txt';

                document.body.appendChild(a);
                a.click();

                window.URL.revokeObjectURL(url);
            });
    }
    
    resetEditor() {
        if (!confirm('Are you sure you want to reset the editor? Any unsaved changes will be lost.')) {
            return;
        }
        this.clearOutputs();
        this.editor.setValue("using System;\nusing System.Linq;\nusing System.Collections.Generic;\n\n");
        this.editor.clearSelection();
        document.getElementById("saveToStorageButton").hidden = false;
        document.getElementById("infoTextBlock").hidden = false;
        document.getElementById("chooseFileButton").hidden = false;
        document.getElementById("executeFileButton").hidden = false;

        document.getElementById("deleteBlobButton").hidden = true;
        document.getElementById("saveBlobButton").hidden = true;
        document.getElementById("resetIcon").hidden = true;
        document.getElementById("fileNameSection").hidden = true;

        this.currentEditingBlobName = null;
        this.refreshBlobFileList();
    }
}