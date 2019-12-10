import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { PrivacyFormComponent } from './privacy-form.component';

const routes: Routes = [
    {
        path: '', component: PrivacyFormComponent,
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class PrivacyFormRoutingModule {
}
