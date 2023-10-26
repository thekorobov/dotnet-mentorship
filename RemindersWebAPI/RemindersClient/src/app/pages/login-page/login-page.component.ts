import { Component } from '@angular/core';
import { Clipboard } from '@angular/cdk/clipboard';
import { HttpService } from 'src/app/api/http.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss'],
  providers: [HttpService]
})

export class LoginPageComponent {
  public showPassword: boolean = false;
  public password: string = "";
  public email: string = "";
  public token: string = "";
  isCopied = false;

  constructor(private router: Router, private httpService: HttpService, private clipboard: Clipboard) {}

  onLoginClick() {
    this.httpService.loginUser(this.email, this.password)
      .subscribe(
        (data: any) => {
          if (data['status'] == 200) {
            this.token = data.body.Data.token;
          }
        },
        (error: any) => {
          console.error('An error occurred:', error);
        }
      );
  }

  onGoogleLoginClick() {
    this.httpService.googleSignIn()
      .subscribe(
        (response: any) => {
          if (response.status === 200) {
            this.token = response.body.Data.token;
          }
        },
        (error: any) => {
          console.error('An error occurred:', error);
        }
      );
  }

  onRegisterClick() {
    this.router.navigateByUrl('/register');
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }

  copyToken() {
    this.clipboard.copy(this.token);
    this.isCopied = true;
    setTimeout(() => this.isCopied = false, 5000);
  }
}
