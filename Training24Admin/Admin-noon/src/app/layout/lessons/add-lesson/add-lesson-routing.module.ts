import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AddLessonComponent } from './add-lesson.component';

const routes: Routes = [
    {
        path: '', component: AddLessonComponent,        
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AddLessonRoutingModule {
}
