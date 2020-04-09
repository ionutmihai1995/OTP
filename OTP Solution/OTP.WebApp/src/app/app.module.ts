import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MaterialModule } from './modules/material.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormHelperService } from './services/form-helper.service';
import { UserService } from './services/user.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { OTPInterceptor } from './services/otp-interceptor';
import { CountdownComponent } from './components/countdown/countdown.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    CountdownComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    FormsModule,
    MaterialModule,
    BrowserAnimationsModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [
    FormHelperService, 
    UserService,
    { provide: HTTP_INTERCEPTORS, useClass: OTPInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
