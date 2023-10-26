import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginPageComponent } from "./pages/login-page/login-page.component";
import { RegisterPageComponent } from "./pages/register-page/register-page.component";

const routes: Routes = [
  {path: '', component: LoginPageComponent, data: {title: 'Login Page', url: '/'}},
  {path: 'register', component: RegisterPageComponent, data: {title: 'Register Page', url: '/'}},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
