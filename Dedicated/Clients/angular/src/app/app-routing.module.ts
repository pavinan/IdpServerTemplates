import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MainNavComponent } from './app/components/main-nav/main-nav.component';
import { AccountModule } from './account/account.module';
import { AuthGuard } from './shared/Auth/auth.guard';
import { SignInCallbackComponent } from './shared/components/sign-in-callback/sign-in-callback.component';


const routes: Routes = [
  {
    path: 'signin-callback',
    component: SignInCallbackComponent
  },
  {
    path: '',
    component: MainNavComponent,
    canActivate: [AuthGuard],
    loadChildren: () => AccountModule
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
