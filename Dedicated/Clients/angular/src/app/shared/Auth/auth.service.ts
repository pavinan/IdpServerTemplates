import { Injectable } from '@angular/core';
import { UserManager, User } from 'oidc-client';
import { ConfigurationService } from '../services/configuration.service';
import { Subject } from 'rxjs/internal/Subject';

@Injectable({
  providedIn: 'root'
})
export class AuthService {



  private authStateChangesSubject = new Subject();
  public authStateChanges = this.authStateChangesSubject.asObservable();

  isAuthenticated = false;
  userManager: UserManager;
  currentUser: User;

  constructor(private configurationService: ConfigurationService) {

    this.userManager = this.createUserManager();
  }

  public signin(url?: string): Promise<void> {
    return this.userManager.signinRedirect({
      state: {
        url
      }
    });
  }

  public signinCallback() {
    this.userManager.signinRedirectCallback().then((loggedInUser) => {

      window.history.replaceState({},
        window.document.title,
        window.location.origin);

      if (loggedInUser.state && loggedInUser.state.url) {
        window.location.href = loggedInUser.state.url;
      } else {
        window.location.href = "/";
      }

      this.authStateChangesSubject.next();
    }, error => {
      console.error(error);
    });
  }

  public signout(): Promise<void> {
    return this.userManager.signoutRedirect();
  }

  private createUserManager() {

    let userManager: UserManager = null;

    const { appUrl, identityUrl } = this.configurationService.configuration;

    userManager = new UserManager({
      authority: identityUrl,
      client_id: 'internal::js',
      scope: 'openid profile email identity',
      response_type: 'code',
      redirect_uri: `${appUrl}/signin-callback`,
      silent_redirect_uri: `${appUrl}/assets/oidc/silent-renew.html`,
      post_logout_redirect_uri: appUrl,
    });

    return userManager;
  }

}
