import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AgentFinComponent } from './agent-fin.component';

const routes: Routes = [
    {
        path: '', component: AgentFinComponent,
        children: [
            { path: '', redirectTo: 'agent-fin-list', pathMatch: 'prefix' },
            { path: 'agent-fin-list', loadChildren: './agent-fin-list/agent-fin-list.module#AgentFinListModule' },
            { path: 'add-deposits', loadChildren: './add-deposits/add-deposits.module#AddDepositsModule' },
            { path: 'edit-deposits/:id', loadChildren: './add-deposits/add-deposits.module#AddDepositsModule' }
        ]
    }    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AgentFinRoutingModule {
}
