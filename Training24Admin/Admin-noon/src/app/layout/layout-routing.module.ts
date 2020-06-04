import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import { LayoutComponent } from "./layout.component";

const routes: Routes = [
    {
        path: "",
        component: LayoutComponent,
        children: [
            { path: "", redirectTo: "users", pathMatch: "prefix" },
            {
                path: "dashboard",
                loadChildren: "./dashboard/dashboard.module#DashboardModule"
            },
            { path: "users", loadChildren: "./users/users.module#UsersModule" },
            {
                path: "bundles",
                loadChildren: "./bundles/bundles.module#BundlesModule"
            },
            {
                path: "grades",
                loadChildren: "./grade/grade.module#GradeModule"
            },
            {
                path: "courses",
                loadChildren: "./courses/courses.module#CoursesModule"
            },
            // { path: 'chapters', loadChildren: './chapters/chapters.module#ChaptersModule' },
            {
                path: "question-types",
                loadChildren:
                    "./question-type/question-type.module#QuestionTypesModule"
            },
            // { path: 'quizs', loadChildren: './quizs/quizs.module#QuizsModule' },
            { path: "files", loadChildren: "./files/files.module#FilesModule" },
            {
                path: "questions",
                loadChildren: "./questions/questions.module#QuestionsModule"
            },
            // { path: 'lessons', loadChildren: './lessons/lessons.module#LessonsModule' },
            {
                path: "assignments",
                loadChildren:
                    "./assignments/assignment.module#AssignmentsModule"
            },
            {
                path: "device-admin",
                loadChildren:
                    "./device-admin/device-admin.module#DeviceAdminModule"
            },
            {
                path: "school",
                loadChildren: "./school/school.module#SchoolModule"
            },
            {
                path: "additional_service",
                loadChildren:
                    "./additional_service/additional_service.module#Additional_ServiceModule"
            },
            {
                path: "package",
                loadChildren: "./package/package.module#PackageModule"
            },
            {
                path:"library",
                loadChildren: "./library/library.module#LibraryModule"
            },
            {
                path:"timeout",
                loadChildren: "./timeout/timeout.module#TimeOutModule"
            },
            {
                path:"tax",
                loadChildren: "./tax/tax.module#TaxModule"
            },
            {
                path:"configure_sales",
                loadChildren: "./sales-configure/sales-configure.module#SalesConfigureModule"
            },
            {
                path:"csvinput",
                loadChildren: "./csvinput/csvinput.module#CsvInputModule"
            },
            {
                path:"app-update",
                loadChildren: "./app-update/app-update.module#AppUpdateModule"
            },
            {
                path:"privacy",
                loadChildren: "./privacy/privacy.module#PrivacyModule"
            }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class LayoutRoutingModule {}
