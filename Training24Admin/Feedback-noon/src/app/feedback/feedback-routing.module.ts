import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FeedbackComponent } from './feedback.component';

const routes: Routes = [
    {
        path: '', component: FeedbackComponent,        
        children: [           
            { path: '', loadChildren: './feedback-form/feedback-form.module#FeedbackFormModule' }        
                      
        ]
        
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class FeedbackRoutingModule {
}
