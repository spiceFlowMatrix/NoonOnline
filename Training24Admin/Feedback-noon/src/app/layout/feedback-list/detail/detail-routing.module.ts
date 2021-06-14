import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FeedbackDetailComponent } from './detail.component';

const routes: Routes = [
    {
        path: '', component: FeedbackDetailComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class FeedbackDetailRoutingModule {
}
