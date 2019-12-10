import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FilesComponent } from './files.component';

const routes: Routes = [
    {
        path: '', component: FilesComponent,
        children: [
            { path: '', redirectTo: 'files-list', pathMatch: 'prefix' },
            { path: 'files-list', loadChildren: './files-list/files-list.module#FilesListModule' },
            { path: 'add-files', loadChildren: './add-files/add-files.module#AddFilesModule' },
            { path: 'edit-files/:id', loadChildren: './add-files/add-files.module#AddFilesModule' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class FilesRoutingModule {
}
