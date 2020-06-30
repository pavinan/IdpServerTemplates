import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../Auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class APIService {

  constructor(
    private httpClient: HttpClient,
    private authService: AuthService) { }


  get<TRes>(url: string, params?: any, headers?: HttpHeaders) {
    const request = this.httpClient.get<TRes>(url, {
      ...this.getOptions(headers),
      params
    });
    return request;
  }


  public post<TRes>(url: string, req?: any, params?: any, headers?: HttpHeaders) {

    if (!params) {
      params = {};
    }

    const request = this.httpClient.post<TRes>(url, req, {
      ...this.getOptions(headers),
      params
    });

    return request;
  }

  public put<TRes>(url: string, req?: any, params?: any, headers?: HttpHeaders) {

    if (!params) {
      params = {};
    }

    const request = this.httpClient.put<TRes>(url, req, {
      ...this.getOptions(headers),
      params
    });

    return request;
  }

  public delete<TRes>(url: string, params?: any, headers?: HttpHeaders) {

    if (!params) {
      params = {};
    }

    const request = this.httpClient.delete<TRes>(url, {
      ...this.getOptions(headers),
      params
    });

    return request;
  }



  private getOptions(headers?: HttpHeaders) {
    const options = {
      headers: headers || new HttpHeaders({
        'Accept': 'application/json'
      })
    };

    if (this.authService.currentUser) {
      options["headers"] = new HttpHeaders()
        .append("Authorization", `Bearer ${this.authService.currentUser.access_token}`);

        options["withCredentials"] = true;
    }

    return options;
  }

}
