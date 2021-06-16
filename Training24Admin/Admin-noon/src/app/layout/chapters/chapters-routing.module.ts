import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ChaptersComponent } from './chapters.component';

const routes: Routes = [
    {
        path: '', component: ChaptersComponent,
        children: [
            { path: '', redirectTo: 'chapters-list', pathMatch: 'prefix' },
            { path: 'chapters-list', loadChildren: './chapters-list/chapters-list.module#ChaptersListModule' },
            { path: 'add-chapter', loadChildren: './add-chapters/add-chapters.module#AddChaptersModule' },
            { path: 'edit-chapter/:id', loadChildren: './add-chapters/add-chapters.module#AddChaptersModule' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ChaptersRoutingModule {
}
