import { ApplicationConfig, isDevMode, provideZonelessChangeDetection } from '@angular/core';
import { errorHandler } from './common/handlers/error.handler';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { AuthInterceptor } from './common/interceptors/auth.interceptor';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { Pagination } from './common/interfaces/pagination';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    errorHandler,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    {
      provide: MatPaginatorIntl,
      useClass: Pagination,
    },
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),
    isDevMode() ? [provideZonelessChangeDetection()] : [],
  ],
};
