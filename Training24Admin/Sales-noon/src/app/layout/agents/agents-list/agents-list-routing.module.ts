import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AgentListComponent } from './agents-list.component';

const routes: Routes = [
    {
        path: '', component: AgentListComponent,
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AgentListRoutingModule {
}
