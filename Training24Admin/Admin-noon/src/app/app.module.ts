import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthGuard, CommonAPIService, DataService } from './shared';
import { AuthService, AuthenticationService } from './shared/services';
import { KatexModule } from 'ng-katex';
import { ToastrModule } from 'ngx-toastr';
import { SatDatepickerModule, SatNativeDateModule } from 'saturn-datepicker';
// AoT requires an exported function for factoriess
export const createTranslateLoader = (http: HttpClient) => {
    /* for development
    return new TranslateHttpLoader(
        http,
        '/start-angular/SB-Admin-BS4-Angular-6/master/dist/assets/i18n/',
        '.json'
    ); */
    return new TranslateHttpLoader(http, './assets/i18n/', '.json');
};

@NgModule({
    imports: [
        BrowserModule,
        CommonModule,
        BrowserAnimationsModule,
        ToastrModule.forRoot(),
        HttpClientModule,
        SatDatepickerModule,
        SatNativeDateModule,
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useFactory: createTranslateLoader,
                deps: [HttpClient]
            }
        }),
        FormsModule,
        AppRoutingModule
    ],
    declarations: [AppComponent],
    providers: [AuthGuard, CommonAPIService, DataService, AuthService,AuthenticationService],
    bootstrap: [AppComponent]
})
export class AppModule { }
