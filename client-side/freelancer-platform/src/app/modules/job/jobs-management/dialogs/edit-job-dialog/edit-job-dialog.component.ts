import { Component, Inject } from '@angular/core';
import { JobService } from '../../../services/job.service';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { EditJobCommand } from '../../../models/commands/edit-job-command.model';
import { Job } from '../../../models/job.model';
import { Profession, Skill } from 'src/app/modules/shared/models/profession.mode';
import { ProfessionService } from 'src/app/modules/shared/services/profession.service';
import { MatSelectChange } from '@angular/material/select';
import { FormControl } from '@angular/forms';
import { MatChipInputEvent, MatChipEditedEvent } from '@angular/material/chips';
import { Question } from '../../../models/question.model';
import { ENTER, COMMA } from '@angular/cdk/keycodes';
import { SnackBarsService } from 'src/app/modules/shared/services/snack-bars.service';

@Component({
  selector: 'app-edit-job-dialog',
  templateUrl: './edit-job-dialog.component.html',
  styleUrls: ['./edit-job-dialog.component.scss']
})
export class EditJobDialogComponent {

  addOnBlur = true;
  readonly separatorKeysCodes = [ENTER, COMMA] as const;
  paymentPanelOpenState = false;

  job: Job = new Job();
  
  professions: Profession[] = [];
  allSkills: Skill[] = [];
  skillsControl = new FormControl<Skill[]>([]);

  editJobCommand: EditJobCommand;

  constructor(
    private jobService: JobService,
    private professionService: ProfessionService,
    private snackBarService: SnackBarsService,
    private dialogRef: MatDialogRef<EditJobDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { job: Job }) {
      this.job = data.job;
      this.editJobCommand = new EditJobCommand(this.job);
  }

  ngOnInit() {
    this.professionService.get().subscribe((professions) => this.professions = professions);
    this.professionService.getSkills(this.job.profession.id).subscribe((skills) => {
      this.allSkills = skills;
      const selectedSkills: Skill[] = this.job.skills.map((skill) => {
        const selectedSkill: Skill | undefined = this.allSkills.find((s) => s.id === skill.id);
        return selectedSkill as Skill;
      });
      this.skillsControl.setValue(selectedSkills);
    });
  }

  edit() {
    this.editJobCommand.skills = (this.skillsControl.value as Skill[]).map((skill) => skill.id);
    this.jobService.edit(this.editJobCommand).subscribe({
      next: this.editSuccessfull.bind(this),
      error: (err) => this.snackBarService.error(err.error[0])
    });
  }

  editSuccessfull(job: Job) {
    Object.assign(this.job, job);
    this.snackBarService.primary('Job edit successful');
    this.dialogRef.close();
  }

  professionSelected(event: MatSelectChange) {
    const professionId = event.value as  string;
    this.professionService.getSkills(professionId).subscribe({
      next: (skills) => this.allSkills = skills
    });
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
      this.editJobCommand.questions.push(new Question(value));
    event.chipInput!.clear();
  }

  removeQuestion(question: Question) {
    const index = this.editJobCommand.questions.indexOf(question);

    if (index >= 0)
      this.editJobCommand.questions.splice(index, 1);
  }

  editQuestion(question: Question, event: MatChipEditedEvent) {
    const value = event.value.trim();

    if (!value) {
      this.removeQuestion(question);
      return;
    }

    const index = this.editJobCommand.questions.indexOf(question);
    if (index >= 0) {
      this.editJobCommand.questions[index].text = value;
    }
  }

  static open(dialog: MatDialog, job: Job): MatDialogRef<EditJobDialogComponent> {
    return dialog.open(EditJobDialogComponent, {
      width: '50%',
      height: '80%',
      data: { job: job }
    })
  }

}
