using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ContactInformation.Contracts;
using ContactInformation.Models;

namespace ContactInformation.APIControllers
{
	public class ContactInformationController : ApiController
	{
		IContactInformationService contactInformationService;

		public ContactInformationController(IContactInformationService contactInformationService)
		{
			this.contactInformationService = contactInformationService;
		}
		[HttpGet]
		public IEnumerable<ContactDetailsViewModel> GetAllUserDetails(bool IncludeInactive = false)
		{
			return contactInformationService.GetAll(IncludeInactive);
		}

		[HttpGet]
		public IEnumerable<ContactDetailsViewModel> GetContactInformationById(int RowId)
		{
			return contactInformationService.GetContactInformationById(RowId);
		}

		[HttpPost]
		[Route("api/ContactInformation/AddContactInformation")]
		public HttpResponseMessage AddContactInformation(ContactDetailsViewModel userDetails)
		{
			int UserId = 0;
			string contentMessage = contactInformationService.AddContactInformation(UserId, userDetails);
			return Request.CreateResponse(HttpStatusCode.OK, contentMessage);
		}
		[HttpPost]
		[Route("api/ContactInformation/UpdateContactInformation")]
		public HttpResponseMessage UpdateContactInformation(ContactInformations userDetails)
		{
			int UserId = 0;
			string contentMessage = contactInformationService.UpdateContactInformation(UserId, userDetails);
			return Request.CreateResponse(HttpStatusCode.OK, contentMessage);
		}

		[HttpDelete]
		[Route("api/ContactInformation/DelteContactInformation")]
		public HttpResponseMessage DelteContactInformation(int Id)
		{
			if (ModelState.IsValid)
			{
				int UserId = 0;
				string contentMessage = contactInformationService.DeleteContactInformation(UserId, Id);
				return Request.CreateResponse(HttpStatusCode.OK, contentMessage);
			}
			else
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unexpected Error..Please contact Administrator.");
			}
		}
	}
}
