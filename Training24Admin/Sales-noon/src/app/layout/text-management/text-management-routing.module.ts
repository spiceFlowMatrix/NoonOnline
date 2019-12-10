import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TextManagementPageComponent } from './text-management.component';

const routes: Routes = [
    {
        path: '',
        component: TextManagementPageComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class TextManagementRoutingModule {}
