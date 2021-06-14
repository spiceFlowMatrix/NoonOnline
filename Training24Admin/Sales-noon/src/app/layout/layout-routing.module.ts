import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from './layout.component';

const routes: Routes = [
    {
        path: '',
        component: LayoutComponent,
        children: [
            { path: '', redirectTo: 'purchases', pathMatch: 'prefix' },
            // { path: 'dashboard', loadChildren: './dashboard/dashboard.module#DashboardModule' },
            { path: 'purchases', loadChildren: './purchases/purchases.module#PurchasesModule' },
            { path: 'courses', loadChildren: './courses/courses.module#CoursesModule' },
            { path: 'agents', loadChildren: './agents/agents.module#AgentsModule' },
            { path: 'agent-finances', loadChildren: './agent-finances/agent-finances.module#AgentFianancesModule' },
            { path: 'agent-fin', loadChildren: './agent-fin/agent-fin.module#AgentFinModule' },
            { path: 'management-info', loadChildren: './text-management/text-management.module#TextManagementPageModule' },
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class LayoutRoutingModule { }
