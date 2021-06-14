import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AgentFinListComponent } from './agent-fin-list.component';

const routes: Routes = [
    {
        path: '', component: AgentFinListComponent,
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AgentFinListRoutingModule {
}
