import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { SignInCallbackComponent } from './components/sign-in-callback/sign-in-callback.component';



@NgModule({
  declarations: [SignInCallbackComponent],
  imports: [
    CommonModule
  ],
  exports: [
    HttpClientModule
  ]
})
export class SharedModule { }
