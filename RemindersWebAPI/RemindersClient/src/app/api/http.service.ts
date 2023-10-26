import { Injectable } from "@angular/core";
import { HttpClient, HttpResponse } from '@angular/common/http';
import { GoogleLoginProvider, SocialAuthService } from 'angularx-social-login';
import { ApiConstants } from "./ApiConstants";
import {from, Observable, switchMap} from "rxjs";

@Injectable()
export class HttpService {
  constructor(private authService: SocialAuthService, private http: HttpClient) { }

  loginUser(email: string, password: string) {
    const headers = { 'content-type': 'application/json' }
    const body = {
      email: email,
      password: password
    };
    var login_url = ApiConstants.main_url.toString() + ApiConstants.login_url.toString()
    return this.http.post(login_url, body, { 'headers': headers, observe: 'response' });
  }

  googleSignIn(): Observable<any> {
    return from(this.authService.signIn(GoogleLoginProvider.PROVIDER_ID))
      .pipe(
        switchMap(user => {
          return this.sendGoogleTokenToServer(user.idToken);
        })
      );
  }

  sendGoogleTokenToServer(googleToken: string) {
    const headers = { 'content-type': 'application/json' };
    const body = {
      Token: googleToken
    };
    const google_login_url = ApiConstants.main_url.toString() + ApiConstants.google_login_url.toString();
    return this.http.post(google_login_url, body, { 'headers': headers, observe: 'response' });
  }

  registerUser(email: string, username: string, password: string) {
    const headers = { 'content-type': 'application/json' }
    const body = {
      email: email,
      username: username,
      password: password
    };
    var register_url = ApiConstants.main_url.toString() + ApiConstants.register_url.toString()
    return this.http.post(register_url, body, { 'headers': headers, observe: 'response' });
  }
}
