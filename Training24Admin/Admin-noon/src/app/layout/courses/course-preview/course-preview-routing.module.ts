import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoursePreviewComponent } from './course-preview.component';
const routes: Routes = [
    {
        path: '', component: CoursePreviewComponent,
        children: [
            { path: 'add-chapter', loadChildren: './../../chapters/add-chapters/add-chapters.module#AddChaptersModule' },
            { path: 'edit-chapter/:id', loadChildren: './../../chapters/add-chapters/add-chapters.module#AddChaptersModule' },
            { path: 'add-lesson', loadChildren: './../../lessons/add-lesson/add-lesson.module#AddLessonModule' },
            { path: 'edit-lesson/:id', loadChildren: './../../lessons/add-lesson/add-lesson.module#AddLessonModule' },
            { path: 'add-quiz', loadChildren: './../../quizs/add-quizs/add-quizs.module#AddQuizsModule' },
            { path: 'edit-quiz/:id', loadChildren: './../../quizs/add-quizs/add-quizs.module#AddQuizsModule' },
            { path: 'add-assignment',loadChildren: './../../assignments/add-assignment/add-assignment.module#AddAssignmentModule'},
            { path: 'edit-assignment/:assignid', loadChildren: './../../assignments/add-assignment/add-assignment.module#AddAssignmentModule' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class CoursePreviewRoutingModule {
}
