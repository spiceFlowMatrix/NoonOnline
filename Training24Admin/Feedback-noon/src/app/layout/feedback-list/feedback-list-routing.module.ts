import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FeedbackListComponent } from './feedback-list.component';
const routes: Routes = [
    {
        path: '', component: FeedbackListComponent,
        children: [
            { path: 'detail/:id', loadChildren: './detail/detail.module#FeedbackDetailModule' },
            { path: 'detail', loadChildren: './detail/detail.module#FeedbackDetailModule' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class FeedbackListRoutingModule {
}
