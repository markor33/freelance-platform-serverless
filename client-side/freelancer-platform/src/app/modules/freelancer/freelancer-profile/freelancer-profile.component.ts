import { Component } from '@angular/core';
import { Availability, Certification, Education, Employment, Freelancer, ProfileSummary } from '../models/freelancer.model';
import { FreelancerService } from '../services/freelancer.service';
import { LanguageProficiencyLevel } from '../../shared/models/language.model';
import { MatDialog } from '@angular/material/dialog';
import { AddEducationDialogComponent } from './dialogs/education/add-education-dialog/add-education-dialog.component';
import { AddCertificationDialogComponent } from './dialogs/certification/add-certification-dialog/add-certification-dialog.component';
import { AddEmploymentDialogComponent } from './dialogs/employment/add-employment-dialog/add-employment-dialog.component';
import { AddSkillDialogComponent } from './dialogs/add-skill-dialog/add-skill-dialog.component';
import { ExperienceLevel } from '../../shared/models/experience-level.model';
import { AuthService } from '../../auth/services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { Feedback } from '../../feedback/models/feedback.model';
import { FeedbackService } from '../../feedback/services/feedback.service';
import { JobInfoDialogComponent } from '../../job/jobs-management/dialogs/job-info-dialog/job-info-dialog.component';
import { SnackBarsService } from '../../shared/services/snack-bars.service';
import { ConfirmationDialogComponent } from '../../shared/dialogs/confirmation-dialog/confirmation-dialog.component';
import { EditCertificationDialogComponent } from './dialogs/certification/edit-certification-dialog/edit-certification-dialog.component';
import { EditEducationDialogComponent } from './dialogs/education/edit-education-dialog/edit-education-dialog.component';
import { EditEmploymentDialogComponent } from './dialogs/employment/edit-employment-dialog/edit-employment-dialog.component';
import { EditProfileSummaryDialogComponent } from './dialogs/edit-profile-summary-dialog/edit-profile-summary-dialog.component';
import { EnumConverter } from '../../shared/utils/enum-string-converter.util';
import { SetProfilePictureDialogComponent } from './dialogs/set-profile-picture-dialog/set-profile-picture-dialog.component';

@Component({
  selector: 'app-freelancer-profile',
  templateUrl: './freelancer-profile.component.html',
  styleUrls: ['./freelancer-profile.component.scss']
})
export class FreelancerProfileComponent {

  profileSummaryHover: boolean = false;
  educationHover: boolean = false;
  certificationHover: boolean = false;
  skillsHover: boolean = false;
  employmentHover: boolean = false;

  role: string = '';
  freelancer: Freelancer = new Freelancer();
  feedbacks: Feedback[] = [];
  freelancerId: string = '';

  constructor(
    private freelancerService: FreelancerService,
    private dialog: MatDialog,
    private authService: AuthService,
    private feedbackService: FeedbackService,
    private route: ActivatedRoute,
    private snackBarService: SnackBarsService,
    public enumConverter: EnumConverter) {
      const id = this.route.snapshot.paramMap.get('id');
      if(id)
        this.freelancerId = id;
      this.role = this.authService.getUserRole();
    }

  ngOnInit() {
    this.freelancerService.get(this.freelancerId).subscribe({
      next: (freelancer) => this.freelancer = freelancer
    });
    this.feedbackService.getByFreelancer(this.freelancerId).subscribe((feedbacks) => this.feedbacks = feedbacks);
  }

  openSetProfilePictureDialog() {
    SetProfilePictureDialogComponent.open(this.dialog);
  }

  openEditProfileSummaryDialog(profileSummary: ProfileSummary) {
    EditProfileSummaryDialogComponent.open(this.dialog, profileSummary);
  }

  openAddEducationDialog() {
    AddEducationDialogComponent.open(this.dialog);
  }

  openEditEducationDialog(education: Education) {
    EditEducationDialogComponent.open(this.dialog, education);
  }

  deleteEducation(educationId: string) {
    const confirmDialog = ConfirmationDialogComponent.open(this.dialog, 'You are about to delete your education.');
    confirmDialog.afterClosed().subscribe((res) => {
      if (!res)
        return;
      this.freelancerService.deleteEducation(educationId).subscribe({
        complete: () => this.snackBarService.primary('Education deleted successfully')
      });
    });
  }

  openAddCertificationDialog() {
    AddCertificationDialogComponent.open(this.dialog);
  }

  openEditCertificationDialog(certification: Certification) {
    EditCertificationDialogComponent.open(this.dialog, certification);
  }

  deleteCertification(certificationId: string) {
    const confirmDialog = ConfirmationDialogComponent.open(this.dialog, 'You are about to delete your certification.');
    confirmDialog.afterClosed().subscribe((res) => {
      if (!res)
        return;
      this.freelancerService.deleteCertification(certificationId).subscribe({
        complete: () => this.snackBarService.primary('Certification deleted successfully')
      });
    });
  }

  openAddEmploymentDialog() {
    AddEmploymentDialogComponent.open(this.dialog);
  }

  openEditEmploymentDialog(employment: Employment) {
    EditEmploymentDialogComponent.open(this.dialog, employment);
  }

  deleteEmployment(employmentId: string) {
    const confirmDialog = ConfirmationDialogComponent.open(this.dialog, 'You are about to delete your employment.');
    confirmDialog.afterClosed().subscribe((res) => {
      if (!res)
        return;
      this.freelancerService.deleteEmployment(employmentId).subscribe({
        complete: () => this.snackBarService.primary('Employment deleted successfully')
      });
    });
  }

  openAddSkillDialog() {
    AddSkillDialogComponent.open(this.dialog, this.freelancer.skills);
  }

  openJobInfoDialog(jobId: string) {
    JobInfoDialogComponent.open(this.dialog, jobId);
  }

  getProfilePictureUrl(): string {
    if (this.freelancer.profilePictureUrl !== null)
      return this.freelancer.profilePictureUrl;
    return 'assets/blank-profile-picture.png';
  }

  getRange(value: number): number[] {
    return Array(Math.floor(value)).fill(0).map((x, i) => i);
  }

}
