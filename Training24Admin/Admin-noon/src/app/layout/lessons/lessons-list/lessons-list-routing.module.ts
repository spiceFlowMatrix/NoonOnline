import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LessonsListComponent } from './lessons-list.component';

const routes: Routes = [
    {
        path: '', component: LessonsListComponent,
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class LessonsListRoutingModule {
}
