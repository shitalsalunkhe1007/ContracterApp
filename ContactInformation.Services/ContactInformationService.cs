using ContactInformation.Contracts;
using ContactInformation.Models;
using ContactInformation.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ContactInformation.Services
{
	public class ContactInformationService : IContactInformationService
	{
		private readonly IUnitOfWork uow;
		private IDataContext dataContext;
		IGenericRepository<ContactInformations> repository;
		public ContactInformationService(IUnitOfWork uow)
		{
			this.uow = uow;
			this.dataContext = uow.DbContext;
		}
		public IEnumerable<ContactDetailsViewModel> GetAll(bool IncludeInactive)
		{
			repository = uow.GetRepository<ContactInformations>();

			IEnumerable<ContactDetailsViewModel> userdetaillist = (from y in repository.Get().Where(x => (IncludeInactive ? true : x.IsActive))
																   select new ContactDetailsViewModel
																   {
																	   Id = y.Id,
																	   FirstName = y.FirstName,
																	   LastName = y.LastName,
																	   EmailId = y.EmailId,
																	   ContactNo = y.ContactNo,
																	   IsActive = y.IsActive
																   }).ToList();

			return userdetaillist;
		}

		public ContactInformations GetByID(int id)
		{
			repository = uow.GetRepository<ContactInformations>();
			var result = repository.GetByID(id);
			uow.DbContext.Entry(result).Reload();
			return result;
		}
		public bool Save()
		{
			bool IsSaved = false;
			try
			{
				IsSaved = uow.Commit();

			}
			catch (Exception ex)
			{
				IsSaved = false;
				throw new Exception(ex.Message);
			}
			return IsSaved;
		}

		public bool Add(ContactInformations entity)
		{
			try
			{
				repository = uow.GetRepository<ContactInformations>();
				repository.Detach(entity);
				repository.Add(entity);
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return true;
		}

		public bool Update(ContactInformations entity)
		{
			try
			{
				entity.Modified = DateTime.Now;
				repository = uow.GetRepository<ContactInformations>();
				ContactInformations existingEmp = repository.GetByID(entity.Id);//entity.OpcoId
				repository.Detach(existingEmp);
				repository.Update(entity);
			}

			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return true;
		}
		public IEnumerable<ContactDetailsViewModel> GetContactInformationById(int Id)
		{
			repository = uow.GetRepository<ContactInformations>();
			IEnumerable<ContactDetailsViewModel> userdetaillist = (from y in repository.Get()
																   where y.Id == Id
																   select new ContactDetailsViewModel
																   {
																	   Id = y.Id,
																	   FirstName = y.FirstName,
																	   LastName = y.LastName,
																	   EmailId = y.EmailId,
																	   ContactNo = y.ContactNo,
																	   IsActive = y.IsActive
																   }).ToList();

			return userdetaillist;
		}
		public string AddContactInformation(int UserId, ContactDetailsViewModel custUserDetails)
		{
			string contentMessage = string.Empty;
			ContactInformations userDetails = new ContactInformations();
			bool DuplicateEntity = false;
			if (custUserDetails.ContactNo == null)
				custUserDetails.ContactNo = string.Empty;
			else
				custUserDetails.ContactNo = custUserDetails.ContactNo.Trim();

			if (custUserDetails.EmailId == null)
				custUserDetails.EmailId = string.Empty;
			else
				custUserDetails.EmailId = custUserDetails.EmailId.Trim();

			DuplicateEntity = CheckDuplicateEntry("EmpId", custUserDetails);

			DuplicateEntity = CheckDuplicateEntry("ContactNo", custUserDetails);
			if (DuplicateEntity)
			{
				contentMessage = "Contact Number already exists";
			}
			else
			{
				DuplicateEntity = CheckDuplicateEntry("EmailId", custUserDetails);
				if (DuplicateEntity)
				{
					contentMessage = "EmailId already exists";
				}
				else
				{

					userDetails.FirstName = custUserDetails.FirstName;
					userDetails.LastName = custUserDetails.LastName;
					userDetails.EmailId = custUserDetails.EmailId;
					userDetails.ContactNo = custUserDetails.ContactNo;
					userDetails.IsActive = custUserDetails.IsActive;
					userDetails.Created = DateTime.Now;
					userDetails.CreatedBy = UserId;
					bool IsAdded = Add(userDetails);

					if (!IsAdded)
					{
						throw new Exception();
					}

					bool IsSaved = Save();
					if (!IsSaved)
					{
						throw new Exception();
					}
					contentMessage = "Record added successfully.";
				}

			}
			return contentMessage;
		}
		public string UpdateContactInformation(int UserId, ContactInformations userDetails)
		{
			string contentMessage = string.Empty;
			var updateEmpObj = GetByID(userDetails.Id);

			if (userDetails.ContactNo == null)
				userDetails.ContactNo = string.Empty;
			else
				userDetails.ContactNo = userDetails.ContactNo.Trim();

			if (userDetails.EmailId == null)
				userDetails.EmailId = string.Empty;
			else
				userDetails.EmailId = userDetails.EmailId.Trim();

			IEnumerable<ContactDetailsViewModel> lstUsers = GetAll(true).Where(x => x.Id != userDetails.Id);
			var DuplicateContactNo = lstUsers.Where(x => x.ContactNo == userDetails.ContactNo && string.IsNullOrEmpty(x.ContactNo) == false);
			var DuplicateEmail = lstUsers.Where(x => x.EmailId != null && x.EmailId.Equals(userDetails.EmailId, StringComparison.OrdinalIgnoreCase));

			if (DuplicateContactNo.Count() > 1)
			{
				contentMessage = "Contact Number already exists";
			}
			else if (DuplicateEmail.Count() > 1)
			{
				contentMessage = "EmailId already exists";
			}
			else
			{

				updateEmpObj.FirstName = userDetails.FirstName;

				updateEmpObj.LastName = userDetails.LastName;
				updateEmpObj.EmailId = userDetails.EmailId;
				updateEmpObj.ContactNo = userDetails.ContactNo;
				updateEmpObj.IsActive = userDetails.IsActive;
				updateEmpObj.Modified = DateTime.Now;
				updateEmpObj.ModifiedBy = UserId;
				bool IsUpdated = Update(updateEmpObj);
				if (!IsUpdated)
				{
					throw new Exception();
				}
				bool updateEmp = Save();
				if (!updateEmp)
				{
					throw new Exception();
				}
				contentMessage = "Record updated successfully.";
			}
			return contentMessage;
		}
		public string DeleteContactInformation(int UserId, int recordId)
		{
			string contentMessage = string.Empty;
			var ContactInformationObj = GetByID(recordId);
			ContactInformationObj.IsActive = false;
			//EmpObj.Modified = DateTime.Now;
			//EmpObj.ModifiedBy = UserId;
			bool isDeleted = Update(ContactInformationObj);
			if (!isDeleted)
			{
				throw new Exception();
			}

			if (isDeleted)
			{
				bool save = Save();
				if (save)
				{
					contentMessage = "Record Deleted successfully.";
				}
				else
				{

					contentMessage = "Record not Deleted successfully.";
				}
			}
			else
			{
				contentMessage = "Record not Deleted successfully.";
			}
			return contentMessage;
		}

		public bool CheckDuplicateEntry(string Entity, ContactDetailsViewModel custUserDetails)
		{
			IEnumerable<ContactDetailsViewModel> lstUsers = GetAll(true);
			switch (Entity)
			{

				case "ContactNo":
					{
						var DuplicateContactNo = lstUsers.Where(x => x.ContactNo == custUserDetails.ContactNo && string.IsNullOrEmpty(x.ContactNo) == false);
						if (DuplicateContactNo.Count() > 0)
							return true;
						else
							return false;
					}
				case "EmailId":
					{
						var DuplicateEmail = lstUsers.Where(x => x.EmailId != null && x.EmailId.Equals(custUserDetails.EmailId, StringComparison.OrdinalIgnoreCase));
						if (DuplicateEmail.Count() > 0)
							return true;
						else
							return false;
					}
			}
			return true;
		}

	}
}
