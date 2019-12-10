import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AgentFinancesComponent } from './agent-finances.component';

const routes: Routes = [
    {
        path: '', component: AgentFinancesComponent,
        children: [
            { path: '', redirectTo: 'agent-finances-list', pathMatch: 'prefix' },
            { path: 'agent-finances-list', loadChildren: './agent-finances-list/agent-finances-list.module#AgentFinancesListModule' },
            { path: 'add-deposit', loadChildren: './add-deposit/add-deposit.module#AddDepositModule' },
            { path: 'edit-deposit/:id', loadChildren: './add-deposit/add-deposit.module#AddDepositModule' }
        ]
    }    
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AgentFianancesRoutingModule {
}
