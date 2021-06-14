import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AgentFinancesListComponent } from './agent-finances-list.component';

const routes: Routes = [
    {
        path: '', component: AgentFinancesListComponent,
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AgentFinancesListRoutingModule {
}
