import { NgModule, ModuleWithProviders } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UtilService } from './services';
import { DirectivesModule } from './modules/directives/directives.module';

@NgModule({
    imports: [
        CommonModule,
        RouterModule,
        FormsModule,
        DirectivesModule,
        HttpClientModule,        
    ],
    declarations: [
        
    ],
    exports: [
        RouterModule,
        CommonModule,
        FormsModule,
        DirectivesModule,
        HttpClientModule
    ],
    entryComponents:[]
})
export class SharedModule {

    static forRoot(): ModuleWithProviders {
        return {
            ngModule: SharedModule,
            providers: [
                // ToastrService,
                UtilService,
            ]
        };
    }
}