import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoursesComponent } from './courses.component';

const routes: Routes = [
    {
        path: '', component: CoursesComponent,
        children: [
            { path: '', redirectTo: 'courses-list', pathMatch: 'full' },
            { path: 'courses-list', loadChildren: './courses-list/courses-list.module#CoursesListModule' },
            { path: 'mycourses', loadChildren: './mycourses/mycourses-list.module#MyCoursesListModule' },
            { path: 'add-course', loadChildren: './add-courses/add-courses.module#AddCoursesModule' },
            { path: 'edit-course/:id', loadChildren: './add-courses/add-courses.module#AddCoursesModule' },
            { path: 'course-preview/:id', loadChildren: './course-preview/course-preview.module#CoursePreviewModule' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class CoursesRoutingModule {
}
