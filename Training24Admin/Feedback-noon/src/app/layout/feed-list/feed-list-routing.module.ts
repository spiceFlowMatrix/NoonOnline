import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FeedListComponent } from './feed-list.component';
const routes: Routes = [
    {
        path: '', component: FeedListComponent,
        children: [
            { path: 'feeddetail/:id', loadChildren: './feed-detail/feed-detail.module#FeedDetailModule' },
            { path: '', loadChildren: './lists/lists.module#ListsModule' },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class FeedListRoutingModule {
}
