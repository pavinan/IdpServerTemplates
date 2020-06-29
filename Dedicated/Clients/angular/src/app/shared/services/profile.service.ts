import { Injectable } from '@angular/core';
import { ConfigurationService } from './configuration.service';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../Auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  constructor(
    private httpClient: HttpClient,
    private authService: AuthService,
    private configurationService: ConfigurationService) {

  }

  get() {

    return this.httpClient.get(this.configurationService.configuration.identityUrl + "/v1.0/users/me", {
      withCredentials: true,
      headers: {
        "Authorization": 'Bearer ' + this.authService.currentUser.access_token
      }
    });

  }

}
