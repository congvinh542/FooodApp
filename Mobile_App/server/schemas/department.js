// import {defineField, defineType} from 'sanity'

export default {
  name: 'department',
  title: 'Departments',
  type: 'document',
  fields: [
   {
      name: 'name',
      type: 'string',
      title: 'Name',
      validation: rule=> rule.required(),
   },
   {
      name: 'description',
      type: 'string',
      title: 'Description',
      validation: rule=> rule.max(200),
   },
   {
      name: 'floor',
      type: 'string',
      title: 'Tầng',
   },
   {
      name: 'Room',
      type: 'string',
      title: 'Number Room',
   },
  ]
}
