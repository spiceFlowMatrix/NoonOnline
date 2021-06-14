import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MyCoursesListComponent } from './mycourses-list.component';

const routes: Routes = [
    {
        path: '', component: MyCoursesListComponent,
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class MyCoursesListRoutingModule {
}
