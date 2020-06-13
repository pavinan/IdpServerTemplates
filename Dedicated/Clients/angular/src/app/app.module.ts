import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, APP_INITIALIZER } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SharedModule } from './shared/shared.module';
import { ConfigurationService } from './shared/services/configuration.service';
import { MainNavComponent } from './app/components/main-nav/main-nav.component';
import { AccountModule } from './account/account.module';

function appInitFactory(configService: ConfigurationService) {
  return () => {
    return new Promise((resolve) => {
      configService.$loaded.subscribe(() => {
        resolve(true);
      })
    })
  }
}


@NgModule({
  declarations: [
    AppComponent,
    MainNavComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    SharedModule,
    AccountModule
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      useFactory: appInitFactory,
      multi: true,
      deps: [ConfigurationService]
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
