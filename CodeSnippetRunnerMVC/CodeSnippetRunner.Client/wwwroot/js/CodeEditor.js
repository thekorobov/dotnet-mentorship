class CodeEditor {
    constructor() {
        this.editor = this.initializeEditor();
        this.isDark = localStorage.getItem("editorThemeIsDark") === "true" ? true : false;
        this.applyTheme();
        this.setThemeIcon();
    }

    initializeEditor() {
        const editor = ace.edit("code-editor");
        editor.setTheme("ace/theme/sqlserver");
        editor.getSession().setMode("ace/mode/csharp");
        editor.setFontSize(18);
        ace.require('ace/ext/language_tools');
        editor.setOptions({
            enableBasicAutocompletion: true,
            enableSnippets: true,
            enableLiveAutocompletion: true
        });
        editor.setValue("using System;\nusing System.Linq;\nusing System.Collections.Generic;\n\n");
        editor.clearSelection();
        editor.navigateTo(4, 0);
        return editor;
    }

    applyTheme() {
        if (this.isDark) {
            this.editor.setTheme("ace/theme/monokai");
        } else {
            this.editor.setTheme("ace/theme/sqlserver");
        }
        this.editor.renderer.updateFull();
    }

    toggleTheme() {
        this.isDark = !this.isDark;
        this.applyTheme();
        localStorage.setItem("editorThemeIsDark", this.isDark);
        const themeIcon = document.getElementById('themeIcon');
        if (this.isDark) {
            themeIcon.style.backgroundImage = 'url(/images/moon.png)';
        } else {
            themeIcon.style.backgroundImage = 'url(/images/sun.png)';
        }
    }

    setThemeIcon() {
        const themeIcon = document.getElementById('themeIcon');
        if (this.isDark) {
            themeIcon.style.backgroundImage = 'url(/images/moon.png)';
        } else {
            themeIcon.style.backgroundImage = 'url(/images/sun.png)';
        }
    }

    clearEditor() {
        if (!confirm('Are you sure you want to clear the editor? Any unsaved changes will be lost.')) {
            return;
        }
        this.editor.setValue("using System;\nusing System.Linq;\nusing System.Collections.Generic;\n\n");
        this.editor.clearSelection();
        this.editor.navigateTo(4, 0);
    }
    
    clearSelection() {
        this.editor.clearSelection();
    }

    getValue() {
        return this.editor.getValue();
    }

    setValue(value) {
        this.editor.setValue(value);
    }
}