import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CsvInputFormComponent } from './csvinput-form.component';

const routes: Routes = [
    {
        path: '', component: CsvInputFormComponent,
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class CsvInputFormRoutingModule {
}
