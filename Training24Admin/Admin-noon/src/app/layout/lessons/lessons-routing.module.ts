import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LessonsComponent } from './lessons.component';

const routes: Routes = [
    {
        path: '', component: LessonsComponent,
        children: [
            { path: '', redirectTo: 'lessons-list', pathMatch: 'prefix' },
            { path: 'lessons-list', loadChildren: './lessons-list/lessons-list.module#LessonListModule' },
            { path: 'add-lesson', loadChildren: './add-lesson/add-lesson.module#AddLessonModule' },
            { path: 'edit-lesson/:id', loadChildren: './add-lesson/add-lesson.module#AddLessonModule' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class LessonsRoutingModule {
}