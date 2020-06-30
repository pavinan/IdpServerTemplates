import { Injectable } from '@angular/core';
import { ConfigurationService } from './configuration.service';
import { APIService } from './api.service';

export interface ApplicationUserModel {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  emailConfirmed: boolean;
  phoneNumber: string;
  phoneNumberConfirmed: boolean;
  twoFactorEnabled: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class ProfileService {

  private url: string;

  constructor(
    private apiSerice: APIService,
    private configurationService: ConfigurationService) {
    this.url = this.configurationService.configuration.identityUrl;
  }

  get() {
    return this.apiSerice.get<ApplicationUserModel>(`${this.url}/api/me`);
  }

}
