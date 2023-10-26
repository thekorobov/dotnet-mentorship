import { Component } from '@angular/core';
import {HttpService} from "../../api/http.service";
import { Router } from '@angular/router';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.scss']
})
export class RegisterPageComponent {
  public showPassword: boolean = false;
  public isRegistered: boolean = false;
  public password: string = "";
  public username: string = "";
  public email: string = "";
  public message: string = "";
  constructor(private router: Router, private httpService: HttpService) {}

  onRegisterClick() {
    this.httpService.registerUser(this.email, this.username, this.password)
      .subscribe(
        (data: any) => {
          if (data['status'] == 200) {
            this.message = "Successful registration!";
            this.isRegistered = true;
          }
        },
        (error: any) => {
          this.message = "Registration failed!";
          console.error('An error occurred:', error);
        }
      );
  }

  onLoginClick() {
    this.router.navigateByUrl('/');
  }

  togglePasswordVisibility() {
    this.showPassword = !this.showPassword;
  }
}
