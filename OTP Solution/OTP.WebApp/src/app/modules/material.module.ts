import { NgModule } from '@angular/core';

import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatListModule } from '@angular/material/list';
const modules = [
  MatCardModule,
  MatInputModule,
  MatButtonModule,
  MatProgressBarModule,
  MatListModule
];

@NgModule({
  imports: modules,
  exports: modules,
})
export class MaterialModule {}
