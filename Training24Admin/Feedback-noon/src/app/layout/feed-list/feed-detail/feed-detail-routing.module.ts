import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FeedDetailComponent } from './feed-detail.component';

const routes: Routes = [
    {
        path: '', component: FeedDetailComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class FeedDetailRoutingModule {
}
