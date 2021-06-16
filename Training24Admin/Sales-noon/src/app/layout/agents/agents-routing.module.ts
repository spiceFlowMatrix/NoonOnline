import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AgentsComponent } from './agents.component';

const routes: Routes = [
    {
        path: '', component: AgentsComponent,
        children: [
            { path: '', redirectTo: 'agents-list', pathMatch: 'prefix' },
            { path: 'agents-list', loadChildren: './agents-list/agents-list.module#AgentListModule' },
            { path: 'add-agent', loadChildren: './add-agent/add-agent.module#AddAgentModule' },
            { path: 'edit-agent/:id', loadChildren: './add-agent/add-agent.module#AddAgentModule' },
            { path: 'add-agent-category', loadChildren: './add-agent-category/add-agent-category.module#AddCategoryModule' },
            { path: 'edit-agent-category/:id', loadChildren: './add-agent-category/add-agent-category.module#AddCategoryModule' }
        ]
    }    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AgentsRoutingModule {
}
