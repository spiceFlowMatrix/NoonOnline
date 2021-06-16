import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from './layout.component';

const routes: Routes = [
    {
        path: '',
        component: LayoutComponent,
        children: [
            { path: '', redirectTo: 'feed-list', pathMatch: 'prefix' },
            { path: 'dashboard', loadChildren: './dashboard/dashboard.module#DashboardModule' },
            { path: 'feedback-list', loadChildren: './feedback-list/feedback-list.module#FeedbackListModule' },
            { path: 'feed-list', loadChildren: './feed-list/feed-list.module#FeedListModule' },
            { path: 'detail/:id', loadChildren: './feedback-detail/feedback-detail.module#FeedbackDetailModule'}
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class LayoutRoutingModule { }
