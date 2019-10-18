using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactInformation.Models;

namespace ContactInformation.Contracts
{
	public interface IContactInformationService
	{
		ContactInformations GetByID(int id);
		IEnumerable<ContactDetailsViewModel> GetAll(bool IncludeInactive);
		bool Save();
		bool Add(ContactInformations entity);
		bool Update(ContactInformations entity);
		IEnumerable<ContactDetailsViewModel> GetContactInformationById(int Id);
		string AddContactInformation(int UserId, ContactDetailsViewModel userDetails);
		string UpdateContactInformation(int UserId, ContactInformations userDetails);
		string DeleteContactInformation(int UserId, int recordId);
	}
}       
