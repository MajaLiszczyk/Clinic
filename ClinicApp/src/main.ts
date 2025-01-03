/// <reference types="@angular/localize" />

import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { TokenInterceptor } from './app/authorization/token-interceptor';


bootstrapApplication(AppComponent, {providers: [provideRouter(routes), provideHttpClient(withInterceptors([TokenInterceptor])),],})
//bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));
