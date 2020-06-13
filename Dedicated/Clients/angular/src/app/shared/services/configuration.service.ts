import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ReplaySubject } from 'rxjs';

export interface Configuration {
  identityUrl: string;
  appUrl: string;
}

@Injectable({
  providedIn: 'root'
})
export class ConfigurationService {

  configuration: Configuration;

  private loadedSubject = new ReplaySubject<boolean>(1);
  $loaded = this.loadedSubject.asObservable();

  constructor(private httpClient: HttpClient) {
    this.get().subscribe(x => {
      this.configuration = x;
      this.loadedSubject.next(true);
    });
  }

  get() {
    return this.httpClient.get<Configuration>("/api/configuration");
  }

}
