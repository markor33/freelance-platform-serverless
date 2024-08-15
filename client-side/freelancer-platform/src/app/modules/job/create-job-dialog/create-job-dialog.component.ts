import { Component } from '@angular/core';
import { CreateJobCommand } from '../models/commands/create-job-command.model';
import { Question } from '../models/question.model';
import {COMMA, ENTER} from '@angular/cdk/keycodes';
import { MatChipEditedEvent, MatChipInputEvent } from '@angular/material/chips';
import { JobService } from '../services/job.service';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { SnackBarsService } from '../../shared/services/snack-bars.service';
import { Profession, Skill } from '../../shared/models/profession.mode';
import { ProfessionService } from '../../shared/services/profession.service';
import { MatSelectChange } from '@angular/material/select';
import { FormControl } from '@angular/forms';
import { Job } from '../models/job.model';

@Component({
  selector: 'app-create-job-dialog',
  templateUrl: './create-job-dialog.component.html',
  styleUrls: ['./create-job-dialog.component.scss']
})
export class CreateJobDialogComponent {

  addOnBlur = true;
  readonly separatorKeysCodes = [ENTER, COMMA] as const;
  paymentPanelOpenState = false;

  professions: Profession[] = [];
  
  allSkills: Skill[] = [];
  skillsControl = new FormControl<Skill[]>([]);

  createJobCommand = new CreateJobCommand();

  constructor(
    private dialogRef: MatDialogRef<CreateJobDialogComponent>,
    private jobService: JobService,
    private snackBars: SnackBarsService,
    private professionService: ProfessionService) {
      this.professionService.get().subscribe((professions) => this.professions = professions);
    }

  create() {
    this.createJobCommand.skills = (this.skillsControl.value as Skill[]).map((skill) => skill.id);
    this.jobService.create(this.createJobCommand).subscribe({
      next: this.jobSuccessfullyAdded.bind(this)
    });
  }

  professionSelected(event: MatSelectChange) {
    const professionId = event.value as  string;
    this.professionService.getSkills(professionId).subscribe({
      next: (skills) => this.allSkills = skills
    });
  }

  jobSuccessfullyAdded(job: Job) {
    this.snackBars.primary('Job successfully added');
    this.dialogRef.close(job);
  }

  removeSkill(skill: Skill) {
    const skills = this.skillsControl.value;
    const index = skills?.indexOf(skill) as number;

    skills?.splice(index, 1);
    this.skillsControl.setValue(skills);
  }

  addQuestion(event: MatChipInputEvent) {
    const value = (event.value || '').trim();
    if (value !== '')
      this.createJobCommand.questions.push(new Question(value));
    event.chipInput!.clear();
  }

  removeQuestion(question: Question) {
    const index = this.createJobCommand.questions.indexOf(question);

    if (index >= 0)
      this.createJobCommand.questions.splice(index, 1);
  }

  editQuestion(question: Question, event: MatChipEditedEvent) {
    const value = event.value.trim();

    if (!value) {
      this.removeQuestion(question);
      return;
    }

    const index = this.createJobCommand.questions.indexOf(question);
    if (index >= 0) {
      this.createJobCommand.questions[index].text = value;
    }
  }

  static open(dialog: MatDialog): MatDialogRef<CreateJobDialogComponent> {
    return dialog.open(CreateJobDialogComponent, {
      width: '50%',
      height: '80%'
    });
  }

}
