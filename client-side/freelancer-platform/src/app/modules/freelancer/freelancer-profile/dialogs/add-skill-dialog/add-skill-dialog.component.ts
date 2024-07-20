import { Component, Inject } from '@angular/core';
import { ProfessionService } from 'src/app/modules/shared/services/profession.service';
import { FreelancerService } from '../../../services/freelancer.service';
import { Skill } from 'src/app/modules/shared/models/profession.mode';
import { FormControl } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { SnackBarsService } from 'src/app/modules/shared/services/snack-bars.service';
import { AddSkillCommand } from '../../../models/commands/add-skill-command.model';

@Component({
  selector: 'app-add-skill-dialog',
  templateUrl: './add-skill-dialog.component.html',
  styleUrls: ['./add-skill-dialog.component.scss']
})
export class AddSkillDialogComponent {

  selectedSkills: Skill[];
  addSkillCommand = new AddSkillCommand();

  allSkills: Skill[] = [];
  skillsControl = new FormControl<Skill[]>([]);

  constructor(
    private dialogRef: MatDialogRef<AddSkillDialogComponent>,
    private professionService: ProfessionService,
    private freelancerService: FreelancerService,
    private snackBars: SnackBarsService,
    @Inject(MAT_DIALOG_DATA) public data: { skills: Skill[] }) {
      this.selectedSkills = data.skills;
     }

  ngOnInit() {
    const professionId = this.freelancerService.currentFreelancer.profession.id;
    this.professionService.getSkills(professionId as string).subscribe((skills) => {
      this.allSkills = skills;
      const selectedSkills: Skill[] = this.selectedSkills.map((skill) => {
        const selectedSkill: Skill | undefined = this.allSkills.find((s) => s.id === skill.id);
        return selectedSkill as Skill;
      });
      this.skillsControl.setValue(selectedSkills);
    });
  }

  remove(skill: Skill) {
    const skills = this.skillsControl.value;
    const index = skills?.indexOf(skill) as number;

    skills?.splice(index, 1);
    this.skillsControl.setValue(skills);
  }

  submit() {
    this.addSkillCommand.skills = (this.skillsControl.value as Skill[]).map((skill) => skill.id);
    this.freelancerService.addSkills(this.addSkillCommand).subscribe({
      complete: this.skillsSuccessfullyAdded.bind(this)
    });
  }

  skillsSuccessfullyAdded() {
    this.snackBars.primary('Skills successfully updated');
    this.dialogRef.close();
  }

  static open(dialog: MatDialog, skills: Skill[]): MatDialogRef<AddSkillDialogComponent> {
    return dialog.open(AddSkillDialogComponent, {
      width: '40%',
      height: '45%',
      data: { skills: skills }
    });
  }

}
