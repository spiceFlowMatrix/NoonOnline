import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LibraryComponent } from './library.component';

const routes: Routes = [
    {
        path: '', component: LibraryComponent,
        children: [
            { path: '', redirectTo: 'book-list', pathMatch: 'prefix' },
            { path: 'book-list', loadChildren: './book-list/book-list.module#BookListModule' },
            { path: 'add-books', loadChildren: './add-books/add-books.module#AddBooksModule' },
            { path: 'edit-book/:id', loadChildren: './add-books/add-books.module#AddBooksModule' }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class LibraryRoutingModule {
}
