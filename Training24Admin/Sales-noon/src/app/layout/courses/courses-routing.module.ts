import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CoursesComponent } from './courses.component';

const routes: Routes = [
    {
        path: '', component: CoursesComponent,
        children: [
            { path: '', redirectTo: 'courses-list', pathMatch: 'prefix' },
            { path: 'courses-list', loadChildren: './courses-list/courses-list.module#CoursesListModule' },
            { path: 'add-course-definition', loadChildren: './add-course-definition/add-course-definition.module#AddCourseDefinitionModule' },
            { path: 'edit-course-definition/:id', loadChildren: './add-course-definition/add-course-definition.module#AddCourseDefinitionModule' },
            { path: 'add-discount-package', loadChildren: './add-discount-package/add-discount-package.module#AddDiscountPackageModule' },
            { path: 'edit-discount-package/:id', loadChildren: './add-discount-package/add-discount-package.module#AddDiscountPackageModule' }
        ]
    }    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class CoursesRoutingModule {
}
